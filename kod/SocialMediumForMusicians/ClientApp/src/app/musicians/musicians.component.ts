import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute } from "@angular/router";
import { Musician } from "./musician";

@Component({
    selector: "app-musicians-list",
    templateUrl: "./musicians.component.html"
})

export class MusiciansComponent implements OnInit {
    public musicians: Musician[];

    constructor(
        private http: HttpClient,
        private activatedRoute: ActivatedRoute,
        @Inject("BASE_URL") private baseUrl: string) { }

    ngOnInit() {
        console.log(this.activatedRoute.snapshot.queryParamMap.get("aaa"));
    }
}