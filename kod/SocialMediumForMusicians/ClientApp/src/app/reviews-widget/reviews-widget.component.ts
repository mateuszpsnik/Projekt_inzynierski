import { Component, OnInit } from "@angular/core";
import { Review } from "../review/review";
import { ReviewService } from "../review/review.service";
import { faStar } from "@fortawesome/free-solid-svg-icons";

@Component({
    selector: "reviews-widget",
    templateUrl: "./reviews-widget.component.html"
})
export class ReviewsWidgetComponent implements OnInit {
    reviews: Array<Review>;
    top: number = 3;

    // Font Awesome Star
    faStar = faStar;

    constructor(
        private service: ReviewService) { }

    ngOnInit() {
        this.getReviews();
    }

    getReviews() {
        this.service.getReviews(this.top).subscribe(result => {
            this.reviews = result.elements;
        }, err => console.error(err));
    }
}