import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ReviewService } from "./review.service";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Review } from "src/models/review";

@Component({
    selector: 'app-add-review-panel',
    templateUrl: './review.component.html'
})
export class ReviewComponent implements OnInit {
    form: FormGroup;

    isAuthenticated: boolean;
    // set by default to true so that when user id
    // is not loaded it does not show the form
    isAnyReview = true;

    authorId: string;
    targetId: string;
    targetName: string;

    constructor(
        private reviewService: ReviewService,
        private authService: AuthorizeService,
        private activatedRoute: ActivatedRoute,
        private router: Router) {}

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            this.targetId = params.id;
        });
        this.activatedRoute.queryParamMap.subscribe(params => {
            this.targetName = params.get('name');
        });

        this.authService.isAuthenticated().subscribe(isAuth => {
            this.isAuthenticated = isAuth;
        });

        if (this.isAuthenticated) {
            this.authService.getUser().subscribe(authUser => {
                this.authorId = authUser.sub;
            });

            this.reviewService.anyReviews(this.targetId, this.authorId).subscribe(result => {
                console.log(result);
                if (result.totalCount > 0) {
                    this.isAnyReview = true;
                } else {
                    this.isAnyReview = false;
                }
            }, err => console.log(err));
        }

        this.form = new FormGroup({
            rate: new FormControl(''),
            description: new FormControl('')
        });
    }

    onSubmit() {
        const review: Review = {
            rate: this.form.get('rate').value,
            description: this.form.get('description').value,
            authorId: this.authorId,
            targetId: this.targetId,
            sentAt: new Date(Date.now())
        };

        this.reviewService.post(review).subscribe(result => {
            console.log(result);
            alert('Opinia została wysłana. Dziękujemy!');
            this.router.navigateByUrl('/');
        }, err => console.error(err));
    }
}
