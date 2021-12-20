import { Component, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Message } from "src/models/message";
import { MessageService } from "../message/message.service";

@Component({
    selector: 'app-messages-thread',
    templateUrl: './messages-thread.component.html',
    styleUrls: ['./messages-thread.component.css']
})
export class MessagesThreadComponent implements OnInit {
    isAuthenticated: boolean;
    userId: string;
    interlocutorId: string;
    messages: Array<Message>;

    form: FormGroup;

    constructor(
        private authService: AuthorizeService,
        private messageService: MessageService,
        private activateRoute: ActivatedRoute) {}

    ngOnInit() {
        this.authService.isAuthenticated().subscribe(isAuth => {
            this.isAuthenticated = isAuth;
        }, err => console.error(err));

        if (this.isAuthenticated) {
            this.authService.getUser().subscribe(authUser => {
                this.userId = authUser.sub;
            }, err => console.error(err));
        }

        this.activateRoute.params.subscribe(params => {
            this.interlocutorId = params.id;
        }, err => console.error(err));

        this.messageService.getMessagesThread(this.userId, this.interlocutorId)
            .subscribe(result => {
                this.messages = result.elements;
                console.log(this.messages);
        }, err => console.error(err));

        this.form = new FormGroup({
            content: new FormControl('', Validators.required)
        });
    }

    onSubmit() {
        const content = this.form.get('content').value;

        if (content !== '') {
            const message: Message = {
                authorId: this.userId,
                recipentId: this.interlocutorId,
                content: content,
                read: false,
                sentAt: new Date(Date.now())
            };

            this.messageService.post(message).subscribe(result => {
                console.log(result);
                alert('Wiadomość została wysłana');
                window.location.reload();
            }, err => console.error(err));
        }
    }
}
