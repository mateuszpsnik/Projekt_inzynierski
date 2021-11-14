import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { UserService } from "../user/user.service";
import { User } from "src/models/User";

@Component({
    selector: 'app-block-component',
    templateUrl: './block.component.html'
})
export class BlockComponent implements OnInit {
    form: FormGroup;
    lockoutEnd: Date;

    adminId: string;
    userId: string;

    user: User;

    constructor(
        private userService: UserService,
        private authService: AuthorizeService,
        private activateRoute: ActivatedRoute) {}

    ngOnInit() {
        this.authService.getUser().subscribe(authUser => {
            this.adminId = authUser.sub;
        }, err => console.error(err));

        this.activateRoute.params.subscribe(params => {
            this.userId = params.id;
        }, err => console.error(err));

        this.userService.get(this.userId).subscribe(user => {
            this.user = user;
        }, err => console.error(err));

        this.form = new FormGroup({
            lockoutEnd: new FormControl('')
        });
    }

    onSubmit() {
        const lockoutEnd = this.form.get('lockoutEnd').value;

        if (lockoutEnd !== '') {
            this.lockoutEnd = new Date(lockoutEnd);

            this.userService.lockout(this.userId, this.adminId, this.lockoutEnd)
                .subscribe(result => {
                    console.log(result);
                }, err => console.error(err));
        }
    }
}
