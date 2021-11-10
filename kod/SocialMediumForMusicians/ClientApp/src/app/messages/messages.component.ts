import { Component, OnInit } from "@angular/core";
import { AuthorizeService } from "src/api-authorization/authorize.service";

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html'
})
export class MessagesComponent implements OnInit {
    isAuthenticated: boolean;
    userId: string;

    constructor(private authService: AuthorizeService) {}

    ngOnInit() {
        this.authService.isAuthenticated().subscribe(isAuth => {
            this.isAuthenticated = isAuth;
        }, err => console.error(err));

        if (this.isAuthenticated) {
            this.authService.getUser().subscribe(authUser => {
                this.userId = authUser.sub;
            }, err => console.error(err));
        }
    }
}
