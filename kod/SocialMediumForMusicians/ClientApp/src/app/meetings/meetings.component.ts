import { Component, OnInit, ViewChild } from "@angular/core";
import { AuthorizeService } from "src/api-authorization/authorize.service";

@Component({
    selector: 'app-meetings',
    templateUrl: './meetings.component.html'
})
export class MeetingsComponent implements OnInit {
    isAuthenticated: boolean;
    userId: string;

    constructor(private authService: AuthorizeService) {}

    ngOnInit() {
        this.authService.isAuthenticated().subscribe(isAuth => {
            this.isAuthenticated = isAuth;
        });

        if (this.isAuthenticated) {
            this.authService.getUser().subscribe(authUser => {
                this.userId = authUser.sub;
            });
        }
    }
}
