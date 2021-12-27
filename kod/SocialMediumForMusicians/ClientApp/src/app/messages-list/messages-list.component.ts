import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { MessageService } from "../message/message.service";
import { Message } from "src/models/message";

@Component({
    selector: 'app-messages-list',
    templateUrl: './messages-list.component.html'
})
export class MessagesListComponent implements OnInit {
    @Input() userId: string;

    messages: Array<Message>;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    defaultPageSize = 3;

    constructor(private messagesService: MessageService) {}

    ngOnInit() {
        const pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = this.defaultPageSize;

        this.getMessages(pageEvent);
    }

    getMessages(event: PageEvent) {
        this.messagesService.getMessages(this.userId, event.pageIndex,
            event.pageSize).subscribe(result => {
                this.messages = result.elements;
                this.paginator.pageIndex = result.pageIndex;
                this.paginator.pageSize = result.pageSize;
                this.paginator.length = result.totalCount;
            }, err => console.error(err));
    }
}
