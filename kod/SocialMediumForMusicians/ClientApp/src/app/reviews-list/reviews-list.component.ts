import { Component, Inject, Input, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Review } from "../../models/review";
import { ReviewService } from "../review/review.service";
import { PaginationApiResult } from 'src/models/pagination_api_result';
import { faStar } from '@fortawesome/free-solid-svg-icons';

@Component({
    selector: "app-reviews-list",
    templateUrl: "./reviews-list.component.html"
})
export class ReviewsListComponent implements OnInit {
    public reviews: Array<Review>;
    @Input() musicianId: string;

    // if this component is used as a list
    // of reviews given by the specified used
    @Input() userId: string;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    public defaultPageSize: number;

    // Font Awesome Star
    faStar = faStar;

    showPaginator = false;

    constructor(
        private service: ReviewService,
        @Inject("BASE_URL")
        private baseUrl: string) { }

    ngOnInit() {
        if (isNaN(this.defaultPageSize)) {
            this.defaultPageSize = 3;
        }

        let pageEvent = new PageEvent();
        // default page index
        pageEvent.pageIndex = 0;
        // default page size
        pageEvent.pageSize = this.defaultPageSize;

        // load reviews list
        this.getElements(pageEvent);
    }

    getElements(event: PageEvent) {
        if (this.userId) {
            this.service.getUserReviewsList(this.userId, event.pageIndex,
                event.pageSize).subscribe(result => {
                    this.paginator.length = result.totalCount;
                    this.paginator.pageIndex = result.pageIndex;
                    this.paginator.pageSize = result.pageSize;
                    this.reviews = result.elements;
                    if (result.elements.length > 0) {
                        this.showPaginator = true;
                    }
                }, err => console.error(err));
        } else {
            this.service.getReviewsList(this.musicianId, event.pageIndex,
                event.pageSize).subscribe(result => {
                    this.paginator.length = result.totalCount;
                    this.paginator.pageIndex = result.pageIndex;
                    this.paginator.pageSize = result.pageSize;
                    this.reviews = result.elements;
                    if (result.elements.length > 0) {
                        this.showPaginator = true;
                    }
                }, err => console.error(err));
        }
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
