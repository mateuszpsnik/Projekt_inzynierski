import { Component, Inject, Input, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Review } from "../review/review";
import { ReviewService } from "../review/review.service";
import { PaginationApiResult } from "../musician/musician.service";

@Component({
    selector: "app-reviews-list",
    templateUrl: "./reviews-list.component.html"
})
export class ReviewsListComponent implements OnInit {
    public reviews: Array<Review>;
    @Input() musicianId: number;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    public defaultPageSize: number;

    constructor(
        private service: ReviewService,
        @Inject("BASE_URL")
        private baseUrl: string) { }
    
    ngOnInit() {
        let pageEvent = new PageEvent();
        // default page index
        pageEvent.pageIndex = 0;
        // default page size
        pageEvent.pageSize = this.defaultPageSize;

        // load reviews list
        this.getElements(pageEvent);
    }
    
    getElements(event: PageEvent) {
        this.service.getReviewsList(this.musicianId, event.pageIndex,
            event.pageSize).subscribe(result => {
                this.paginator.length = result.totalCount;
                this.paginator.pageIndex = result.pageIndex;
                this.paginator.pageSize = result.pageSize;
                this.reviews = result.elements;
            }, err => console.error(err));
    }

    onSubmit() {
        // reload the list, filtering and sorting the reviews
        let pageEvent = new PageEvent();
        // default page index
        pageEvent.pageIndex = 0;
        // default page size
        pageEvent.pageSize = this.defaultPageSize;

        this.getElements(pageEvent);
    }
}