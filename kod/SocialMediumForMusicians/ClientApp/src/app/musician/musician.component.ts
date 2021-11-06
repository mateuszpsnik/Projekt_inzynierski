import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { Musician } from '../../models/musician';
import { MusicianService } from './musician.service';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { faStarHalf } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../user/user.service';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { User } from 'src/models/User';

@Component({
    selector: 'app-musician',
    templateUrl: './musician.component.html'
})
export class MusicianComponent implements OnInit {
    id: number;
    musician: Musician;

    formMessage: FormGroup;
    // Font Awesome Star
    faStar = faStar;
    faHalfStar = faStarHalf;

    public isAuthenticated: boolean;
    public userId: string;
    private user: User;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private service: MusicianService,
        private userService: UserService,
        private authorizeService: AuthorizeService) { }

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            this.id = params.id;
        });
        this.service.get<Musician>(this.id).subscribe(musician => {
            this.musician = musician;
        });

        this.formMessage = new FormGroup({
            content: new FormControl(''),
            email: new FormControl('')
        });

        this.authorizeService.isAuthenticated().subscribe(isAuthenticated => {
            this.isAuthenticated = isAuthenticated;
        });

        this.authorizeService.getUser().subscribe(authUser => {
            this.userId = authUser.sub;
        });
    }

    refreshComponent() {
        // reload the component when redirected to a different musician
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate([`/Musician/${this.id}`]);
        });
    }


    onAddToFavourites() {
        this.userService.get(this.userId).subscribe(user => {
            if (user.favouriteMusiciansIds == null) {
                user.favouriteMusiciansIds = [];
            }
            user.favouriteMusiciansIds.push(this.musician.id);
            this.userService.put(user).subscribe(result => {
                console.log(result);
                alert('Muzyk został dodany do ulubionych.');
            }, err => console.error(err));
        }, err => console.error(err));
    }

    onSubmitMessage() {}
}
