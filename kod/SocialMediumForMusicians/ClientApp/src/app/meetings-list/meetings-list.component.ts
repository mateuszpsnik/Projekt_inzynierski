import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { MeetingService } from "../meeting/meeting.service";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Meeting } from "src/models/meeting";
import { MatPaginator, PageEvent } from "@angular/material/paginator";

@Component({
    selector: 'app-meetings-list',
    templateUrl: './meetings-list.component.html'
})
export class MeetingsListComponent implements OnInit {
    @Input() userId: string;
    @Input() isHost: boolean;
    @Input() isGuest: boolean;

    meetings: Array<Meeting> = [];

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    defaultPageSize = 3;

    constructor(private meetingService: MeetingService) {}

    ngOnInit() {
        const pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = this.defaultPageSize;

        // I don't know why but the first
        // getMeetings() doesn't set the paginator
        this.getMeetings(pageEvent);
        this.getMeetings(pageEvent);

        console.log('meeting', this.meetings);
    }

    getMeetings(event: PageEvent) {
        if (this.isHost) {
            this.meetingService.getMeetings(this.userId, null, event.pageIndex,
                event.pageSize).subscribe(result => {
                    this.meetings = result.elements;
                    this.paginator.pageIndex = result.pageIndex;
                    this.paginator.pageSize = result.pageSize;
                    this.paginator.length = result.totalCount;
                    // console.log(this.meetings);
                }, err => console.error(err));
        } else if (this.isGuest) {
            this.meetingService.getMeetings(null, this.userId, event.pageIndex,
                event.pageSize).subscribe(result => {
                    this.meetings = result.elements;
                    this.paginator.pageIndex = result.pageIndex;
                    this.paginator.pageSize = result.pageSize;
                    this.paginator.length = result.totalCount;
                }, err => console.error(err));
        }
    }
}
