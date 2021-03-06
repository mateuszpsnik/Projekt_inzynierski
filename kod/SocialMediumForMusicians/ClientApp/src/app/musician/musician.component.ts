import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Musician } from '../../models/musician';
import { MusicianService } from './musician.service';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { faStarHalf } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../user/user.service';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { User } from 'src/models/User';
import { MessageService } from '../message/message.service';
import { EmailMessageService } from '../message/email-message.service';
import { EmailMessage, Message } from 'src/models/message';
import { Guid } from 'src/models/guid';

@Component({
    selector: 'app-musician',
    templateUrl: './musician.component.html',
    styleUrls: ['./musician.component.css']
})
export class MusicianComponent implements OnInit {
    // id of the Musician shown to the user
    id: string;
    musician: Musician;

    formMessage: FormGroup;
    // Font Awesome Star
    faStar = faStar;
    faHalfStar = faStarHalf;

    public isAuthenticated: boolean;
    // id of the authenticated user
    public userId: string;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private service: MusicianService,
        private userService: UserService,
        private authorizeService: AuthorizeService,
        private messageService: MessageService,
        private emailMessageService: EmailMessageService) { }

    ngOnInit() {
        this.activatedRoute.params.subscribe(params => {
            this.id = params.id;
        });
        this.service.get(this.id).subscribe(musician => {
            this.musician = musician;
            console.log(this.musician);
        });

        this.formMessage = new FormGroup({
            content: new FormControl('', [ Validators.required, Validators.maxLength(2000) ]),
            email: new FormControl('', [ Validators.email ])
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
            if (user.favouriteMusiciansIds.includes(this.musician.id)) {
                alert('Muzyk ju?? zosta?? dodany do ulubionych.');
            } else {
                user.favouriteMusiciansIds.push(this.musician.id);
                this.userService.put(user).subscribe(result => {
                    // console.log(result);
                    alert('Muzyk zosta?? dodany do ulubionych.');
                }, err => console.error(err));
            }
            console.log(user);
        }, err => console.error(err));
    }

    onSubmitMessage() {
        const content = this.formMessage.get('content').value;
        const emailAddress = this.formMessage.get('email').value;

        if (this.isAuthenticated) {
            if (content !== '') {
                const message: Message = {
                    authorId: this.userId,
                    recipentId: this.id,
                    content: content,
                    read: false,
                    sentAt: new Date(Date.now())
                };

                this.messageService.post(message).subscribe(result => {
                    console.log(result);
                    alert('Wiadomo???? zosta??a wys??ana');
                    this.formMessage.reset();
                }, err => console.error(err));
            } else {
                alert('Podaj tre???? wiadomo??ci');
            }
        } else {
            if (content !== '' && emailAddress !== '') {
                const message: EmailMessage = {
                    authorEmail: emailAddress,
                    recipentId: this.id,
                    content: content,
                    read: false,
                    sentAt: new Date(Date.now())
                };

                this.emailMessageService.post(message).subscribe(result => {
                    console.log(result);
                    alert('Wiadomo???? zosta??a wys??ana');
                    this.formMessage.reset();
                }, err => console.error(err));
            } else {
                alert('Podaj poprawny adres email');
            }
        }
    }
}
