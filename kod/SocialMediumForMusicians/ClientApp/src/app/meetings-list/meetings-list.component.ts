import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { MeetingService } from "../meeting/meeting.service";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { Meeting } from "src/models/meeting";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Router } from "@angular/router";

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

    constructor(private meetingService: MeetingService,
        private router: Router) {}

    ngOnInit() {
        const pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = this.defaultPageSize;

        // I don't know why but the first
        // getMeetings() doesn't set the paginator
        this.getMeetings(pageEvent);
        this.getMeetings(pageEvent);
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

    accept(meeting: Meeting) {
        meeting.accepted = true;
        this.meetingService.put(meeting).subscribe(result => {
            console.log(result);

            const pageEvent = new PageEvent();
            pageEvent.pageIndex = 0;
            pageEvent.pageSize = this.defaultPageSize;
            this.getMeetings(pageEvent);
        }, err => console.error(err));
    }

    cancel(meeting: Meeting) {
        this.meetingService.delete(meeting).subscribe(result => {
            console.log(result);

            const pageEvent = new PageEvent();
            pageEvent.pageIndex = 0;
            pageEvent.pageSize = this.defaultPageSize;
            this.getMeetings(pageEvent);
        }, err => console.error(err));
    }
}
