import { Component, Inject, Input, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AbstractControl, AsyncValidatorFn, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Musician } from "../../models/musician";
import { MusicianService } from "../musician/musician.service";
import { InstrumentItem } from "../home-form/home-form.component";
import { HttpClient } from "@angular/common/http";
import { PaginationApiResult } from "src/models/pagination_api_result";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";

@Component({
    selector: "app-musicians-list",
    templateUrl: "./musicians.component.html",
    styleUrls: ['./musicians.component.css']
})
export class MusiciansComponent implements OnInit {
    public musicians: Musician[];
    // form model
    form: FormGroup;
    options: Array<InstrumentItem>;
    musicianType: number = null;
    @Input() instrument: string = null;
    @Input() musicianId: number = null;
    minPrice: number = 0.0;
    maxPrice: number = 1000.0;
    avgScore: number = 0.0;
    sort: number = 0;
    @ViewChild(MatPaginator)
    private paginator: MatPaginator;    
    public defaultPageSize: number;
    public isCollapsed = true;

    constructor(
        private activatedRoute: ActivatedRoute,
        private service: MusicianService,
        private http: HttpClient,
        @Inject("BASE_URL") 
        private baseUrl: string) { }

    ngOnInit() {
        this.fillOptions();
        this.form = new FormGroup({
            musicianTypeRadio: new FormControl(""),
            instrumentsList: new FormControl(""),
            min: new FormControl("", [ Validators.min(0), Validators.max(1000) ]),
            max: new FormControl("", [ Validators.min(0), Validators.max(1000) ]),
            avg: new FormControl("", [ Validators.min(0), Validators.max(5) ]),
            sortRadio: new FormControl("")
        }, null, this.isMinMaxInvalid());

        this.musicianType = parseInt(this.activatedRoute.snapshot.queryParamMap
                                .get("type"));
        if (isNaN(this.musicianType)) {
            this.musicianType = null;
        }
        if (this.instrument == null) {
            this.instrument = this.activatedRoute.snapshot.queryParamMap
                                  .get("instrument");
        }
        this.minPrice = +this.activatedRoute.snapshot.queryParamMap
                                .get("minPrice");
        this.maxPrice = +this.activatedRoute.snapshot.queryParamMap
                                .get("maxPrice");
        this.avgScore = +this.activatedRoute.snapshot.queryParamMap
                                .get("minAvgScore");
        this.defaultPageSize = parseInt(this.activatedRoute.snapshot.queryParamMap
                                .get("pageSize"));
        if (isNaN(this.defaultPageSize)) {
            this.defaultPageSize = 3;
        }
        if (this.maxPrice == 0) {
            this.maxPrice = 1000;
        }      

        let pageEvent = new PageEvent();
        // default page index
        pageEvent.pageIndex = 0;
        // default page size
        pageEvent.pageSize = this.defaultPageSize;

        // load musicians list
        this.getElements(pageEvent);

        // fill the filter/sort form
        this.fillForm();
    }

    fillForm() {
        this.form.setValue({
            musicianTypeRadio: this.musicianType != null ? this.musicianType.toString() : "",
            instrumentsList: null,
            min: this.minPrice != 0 ? this.minPrice.toString() : "",
            max: this.maxPrice != 1000 ? this.maxPrice.toString() : "",
            avg: this.avgScore != 0 ? this.avgScore.toString() : "",
            sortRadio: this.sort.toString()
        });
    }

    fillOptions() {
        let url = this.baseUrl + "api/Instruments";
        this.http.get<InstrumentItem[]>(url).subscribe(result => {
            this.options = result;
            console.log(this.options);
        }, err => console.log(err));
    }

    getElements(event: PageEvent) {
        this.service.getMusicians<PaginationApiResult<Musician>>(event.pageIndex,
                event.pageSize, this.musicianType, this.instrument, this.minPrice,
                this.maxPrice, this.avgScore, this.sort)
                    .subscribe(result => {
            this.paginator.length = result.totalCount;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
            this.musicians = result.elements;
        }, err => console.error(err));
    }

    onSubmit() {
        // get data from the form
        if (this.form.get("musicianTypeRadio").value) {
            this.musicianType = this.form.get("musicianTypeRadio").value;
            console.log(this.musicianType);
        }
        if (this.form.get("instrumentsList").value) {
            this.instrument = this.form.get("instrumentsList").value;
            console.log(this.instrument);
        }
        if (this.form.get("min").value) {
            this.minPrice = this.form.get("min").value;
            console.log(this.minPrice);
        }
        if (this.form.get("max").value) {
            this.maxPrice = this.form.get("max").value;
            console.log(this.maxPrice);
        }
        if (this.form.get("avg").value) {
            this.avgScore = this.form.get("avg").value;
            console.log(this.avgScore);
        }
        if (this.form.get("sortRadio").value) {
            this.sort = this.form.get("sortRadio").value;
            console.log(this.sort);
        }

        // reload the list, filtering and sorting the musicians
        let pageEvent = new PageEvent();
        // default page index
        pageEvent.pageIndex = 0;
        // default page size
        pageEvent.pageSize = this.defaultPageSize;

        this.getElements(pageEvent);

        // collapse the filtering/sorting panel
        this.isCollapsed = true;
    }

    getMinMax() {
        let min = 0;
        let max = 1000;
        if (this.form) {
            if (this.form.get("min").value !== null && 
                    this.form.get("min").value !== "") {
                min = this.form.get("min").value;
            }
            if (this.form.get("max").value !== null && 
                    this.form.get("max").value !== "") {
                max = this.form.get("max").value;
            }
        }

        return [min, max];
    }

    // returns true if values 'min' and 'max' are invalid
    checkValues(): Observable<boolean> {
        const [min, max] = this.getMinMax();

        console.log(min, max);

        return of(min >= max);
    }

    isMinMaxInvalid(): AsyncValidatorFn {
        return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
            return this.checkValues().pipe(map(result => {
                            return (result ? { isMinMaxInvalid: true } : null);
                    }));
        };
    }
}

