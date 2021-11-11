import { Component, OnInit } from "@angular/core";
import { AuthorizeService } from "src/api-authorization/authorize.service";

@Component({
    selector: 'app-reviews-user',
    templateUrl: './reviews-user.component.html'
})
export class ReviewsUserComponent implements OnInit {
    id: string;

    constructor(
        private authorizeSerice: AuthorizeService) {}

    ngOnInit() {
        this.authorizeSerice.getUser().subscribe(authUser => {
            this.id = authUser.sub;
        });
    }
}
