import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, AsyncValidatorFn, AbstractControl, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ReviewService } from "./review.service";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Review } from "src/models/review";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Component({
    selector: 'app-add-review-panel',
    templateUrl: './review.component.html',
    styleUrls: ['./review.component.css']
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
        }, err => console.error(err));
        this.activatedRoute.queryParamMap.subscribe(params => {
            this.targetName = params.get('name');
        }, err => console.error(err));

        this.authService.isAuthenticated().subscribe(isAuth => {
            this.isAuthenticated = isAuth;
        }, err => console.error(err));

        if (this.isAuthenticated) {
            this.authService.getUser().subscribe(authUser => {
                this.authorId = authUser.sub;
            }, err => console.error(err));

            this.reviewService.anyReviews(this.targetId, this.authorId).subscribe(result => {
                console.log(result);
                if (result.totalCount > 0) {
                    this.isAnyReview = true;
                } else {
                    this.isAnyReview = false;
                }
            }, err => console.error(err));
        }

        this.form = new FormGroup({
            rate: new FormControl('', [ Validators.required,
                Validators.min(1), Validators.max(5) ]),
            description: new FormControl('', [ Validators.required, Validators.maxLength(200) ])
        }, null, this.isNotInRange());
    }

    getReview() {
        const review: Review = {
            rate: this.form.get('rate').value,
            description: this.form.get('description').value,
            authorId: this.authorId,
            targetId: this.targetId,
            sentAt: new Date(Date.now())
        };

        return review;
    }

    onSubmit() {
        const review = this.getReview();

        this.reviewService.post(review).subscribe(result => {
            console.log(result);
            alert('Opinia została wysłana. Dziękujemy!');
            this.router.navigateByUrl('/');
        }, err => console.error(err));
    }

    isNotInRange(): AsyncValidatorFn {
        return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
            const review = this.getReview();
            return this.reviewService.isNotInRange(review)
                        .pipe(map(result => {
                            return (result ? { isNotInRange: true } : null);
                    }));
        };
    }
}
