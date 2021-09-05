import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Musician } from "../musician/musician";
import { MusicianService, PaginationApiResult } from "../musician/musicians.service";

@Component({
    selector: "app-musicians-list",
    templateUrl: "./musicians.component.html"
})
export class MusiciansComponent implements OnInit {
    public musicians: Musician[];
    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    public defaultPageSize: number;
    musicianType: number = null;
    instrument: string = null;
    minPrice: number = 0.0;
    maxPrice: number = 1000.0;
    avgScore: number = 0.0;

    constructor(
        private activatedRoute: ActivatedRoute,
        private service: MusicianService) { }

    ngOnInit() {
        this.musicianType = parseInt(this.activatedRoute.snapshot.queryParamMap
                                .get("type"));
        if (isNaN(this.musicianType)) {
            this.musicianType = null;
        }
        this.instrument = this.activatedRoute.snapshot.queryParamMap
                                .get("instrument");
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
    }

    getElements(event: PageEvent) {
        this.service.getMusicians<PaginationApiResult<Musician>>(event.pageIndex,
                event.pageSize, this.musicianType, this.instrument, this.minPrice,
                this.maxPrice, this.avgScore)
                    .subscribe(result => {
            this.paginator.length = result.totalCount;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
            this.musicians = result.elements;
            console.log(this.musicians);
        }, err => console.error(err));
    }
}