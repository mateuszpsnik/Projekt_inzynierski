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
    constructor(
        private activatedRoute: ActivatedRoute,
        private service: MusicianService) { }

    ngOnInit() {
        this.defaultPageSize = 3;

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
                event.pageSize)
                    .subscribe(result => {
            this.paginator.length = result.totalCount;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
            this.musicians = result.elements;
            console.log(this.musicians);
        }, err => console.error(err));
    }
}