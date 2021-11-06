import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import { Musician } from '../../models/musician';
import { MusicianService } from './musician.service';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { faStarHalf } from '@fortawesome/free-solid-svg-icons';

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

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private service: MusicianService) { }

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            this.id = params.id;
        });
        this.service.get<Musician>(this.id).subscribe(result => {
            this.musician = result;
        });

        this.formMessage = new FormGroup({
            content: new FormControl(''),
            email: new FormControl('')
        });
    }

    refreshComponent() {
        // reload the component when redirected to a different musician
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate([`/Musician/${this.id}`]);
        });
    }


    onAddToFavourites() {
        alert('dupa');
    }

    onSubmitMessage() {}
}
