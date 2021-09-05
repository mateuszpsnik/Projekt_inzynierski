import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Router } from "@angular/router";
import { FormGroup, FormControl } from "@angular/forms";

interface InstrumentItem {
    id: number;
    name: string;
}

@Component({
    selector: "app-home-form",
    templateUrl: "./home-form.component.html"
})
export class HomeFormComponent implements OnInit {
    // form model
    form: FormGroup;
    options: Array<InstrumentItem>;
    musicianType: number = null;
    instrument: string = null;
    minPrice: number = 0.0;
    maxPrice: number = 1000.0;
    avgScore: number = 0.0;

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
            from: new FormControl(""),
            to: new FormControl(""),
            avgScore: new FormControl("")
        });
    }

    fillOptions() {
        let url = this.baseUrl + "api/Instruments";
        this.http.get<InstrumentItem[]>(url).subscribe(result => {
            this.options = result;
            console.log(this.options);
        }, err => console.log(err));
    }

    onSubmit() {
        // get data from the form
        if (this.form.get("btnradio").value != "") {
            this.musicianType = this.form.get("btnradio").value;
        }
        if (this.form.get("instruments").value != "") {
            this.instrument = this.form.get("instruments").value;
        }
        if (this.form.get("from").value != "") {
            this.minPrice = this.form.get("from").value;
        }
        if (this.form.get("to").value != "") {
            this.maxPrice = this.form.get("to").value;
        }
        if (this.form.get("avgScore").value != "") {
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
}