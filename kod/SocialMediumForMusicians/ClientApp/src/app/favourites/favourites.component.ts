import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Musician } from "src/models/musician";
import { MusicianService } from "../musician/musician.service";
import { UserService } from "../user/user.service";

@Component({
    selector: 'app-favourites',
    templateUrl: './favourites.component.html'
})
export class FavouritesComponent implements OnInit {
    userId: string;

    favouriteMusiciansIds: Array<string>;
    favouriteMusicians: Array<Musician> = [];
    defaultPageSize = 3;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;

    constructor(
        private authService: AuthorizeService,
        private userService: UserService,
        private musicianService: MusicianService) {}

    ngOnInit() {
        this.authService.getUser().subscribe(authUser => {
            this.userId = authUser.sub;
        }, err => console.error(err));

        this.getElements();

        console.log(this.favouriteMusicians);
    }

    getElements() {
        this.userService.get(this.userId).subscribe(user => {
            user.favouriteMusiciansIds.forEach(musicianId => {
                this.musicianService.get(musicianId).subscribe(musician => {
                    this.favouriteMusicians.push(musician);
                });
            });
        }, err => console.log(err));
    }
}
