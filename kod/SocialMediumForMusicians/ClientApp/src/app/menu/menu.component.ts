import { Component, OnInit } from "@angular/core";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { UserService } from "../user/user.service";

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html'
})
export class MenuComponent implements OnInit {
    isVisible = false;

    userId: string;
    isMusician: boolean;

    constructor(
        private authorizeService: AuthorizeService,
        private userService: UserService) {}

    ngOnInit() {
        this.authorizeService.getUser().subscribe(authUser => {
            this.userId = authUser.sub;
        }, err => console.error(err));

        if (this.userId) {
            this.userService.get(this.userId).subscribe(user => {
                this.isMusician = user.isMusician;
                console.log(user);
            }, err => console.error(err));
        }
    }

    clicked() {
        this.isVisible = !this.isVisible;
    }
}
