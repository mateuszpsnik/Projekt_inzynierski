import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { EmailMessage } from "src/models/message";
import { EmailMessageService } from "../message/email-message.service";

@Component({
    selector: 'app-email-messages-list',
    templateUrl: './email-messages-list.component.html'
})
export class EmailMessagesListComponent implements OnInit {
    @Input() userId: string;

    messages: Array<EmailMessage>;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    defaultPageSize = 3;

    constructor(private emailMessagesService: EmailMessageService) {}

    ngOnInit() {
        const pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = this.defaultPageSize;

        this.getMessages(pageEvent);
    }

    getMessages(event: PageEvent) {
        this.emailMessagesService.getMessages(this.userId, event.pageIndex,
            event.pageSize).subscribe(result => {
                this.messages = result.elements;
                this.paginator.pageIndex = result.pageIndex;
                this.paginator.pageSize = result.pageSize;
                this.paginator.length = result.totalCount;
                console.log(`Messages: ${result.elements}`);
            }, err => console.error(err));
    }

    onClick() {
        alert('Wiadomość pochodzi od niezarejestrowanego użytkownika - ' +
            'nie możesz na nią odpowiedzieć tutaj. Jeśli chcesz odpowiedzieć, ' +
            'wyślij samodzielnie email na podany adres.');
    }
}
