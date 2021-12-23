import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators, AsyncValidatorFn, AbstractControl } from "@angular/forms";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";

export interface InstrumentItem {
    id: number;
    name: string;
}

@Component({
    selector: "app-home-form",
    templateUrl: "./home-form.component.html",
    styleUrls: ["./home-form.component.css"]
})
export class HomeFormComponent implements OnInit {
    // form model
    form: FormGroup;
    options: Array<InstrumentItem>;
    musicianType: number = null;
    instrument: string = null;
    minPrice = 0.0;
    maxPrice = 1000.0;
    avgScore = 0.0;

    constructor(
        private router: Router,
        private http: HttpClient,
        @Inject("BASE_URL") 
        private baseUrl: string) { }

    ngOnInit() {
        this.fillOptions();
        this.form = new FormGroup({
            btnradio: new FormControl(""),
            instruments: new FormControl(""),
            from: new FormControl("", [ Validators.min(0), Validators.max(1000) ]),
            to: new FormControl("", [ Validators.min(0), Validators.max(1000) ]),
            avgScore: new FormControl("", [ Validators.min(0), Validators.max(5) ])
        }, null, this.isToFromInvalid());
    }

    fillOptions() {
        let url = this.baseUrl + "api/Instruments";
        this.http.get<InstrumentItem[]>(url).subscribe(result => {
            this.options = result;
            console.log(this.options);
        }, err => console.log(err));
    }

    getFromTo() {
        let from = 0;
        let to = 1000;
        if (this.form) {
            if (this.form.get("from").value !== null && 
                    this.form.get("from").value !== "") {
                from = this.form.get("from").value;
            }
            if (this.form.get("to").value !== null && 
                    this.form.get("to").value !== "") {
                to = this.form.get("to").value;
            }
        }

        return [from, to];
    }

    onSubmit() {
        // get data from the form
        if (this.form.get("btnradio").value) {
            this.musicianType = this.form.get("btnradio").value;
        }
        if (this.form.get("instruments").value) {
            this.instrument = this.form.get("instruments").value;
        }
        if (this.form.get("from").value) {
            this.minPrice = this.form.get("from").value;
        }
        if (this.form.get("to").value) {
            this.maxPrice = this.form.get("to").value;
        }
        if (this.form.get("avgScore").value) {
            this.avgScore = this.form.get("avgScore").value;
        }

        // set parameters and redirect
        this.router.navigate(["/Musicians"], { queryParams: {
             type: this.musicianType ? this.musicianType.toString() : null,
             instrument: this.instrument,
             minPrice: this.minPrice.toString(),
             maxPrice: this.maxPrice.toString(),
             minAvgScore: this.avgScore.toString(),
             pageSize: 5
        }});
    }

    // returns true if values 'from' and 'to' are invalid
    checkValues(): Observable<boolean> {
        const [from, to] = this.getFromTo();

        console.log(from, to);

        return of(from >= to);
    }

    isToFromInvalid(): AsyncValidatorFn {
        return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
            return this.checkValues().pipe(map(result => {
                            return (result ? { isToFromInvalid: true } : null);
                    }));
        };
    }
}
