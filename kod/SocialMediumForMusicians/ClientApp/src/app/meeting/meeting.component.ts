import { Component, Input, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { MeetingService } from "./meeting.service";
import { Meeting } from "src/models/meeting";

@Component({
    selector: 'app-meeting-planner',
    templateUrl: './meeting.component.html'
})
export class MeetingComponent implements OnInit {
    form: FormGroup;
    start: Date;
    end: Date;
    notes: string;

    @Input() musicianId: string;
    @Input() userId: string;

    constructor(
        private service: MeetingService,
        private authService: AuthorizeService) {}

    ngOnInit() {
        this.form = new FormGroup({
            day: new FormControl(''),
            startTime: new FormControl(''),
            endTime: new FormControl(''),
            notes: new FormControl('')
        });
    }

    onSubmit() {
        const day = this.form.get('day').value;
        const startTime = this.form.get('startTime').value;
        const endTime = this.form.get('endTime').value;
        const notes = this.form.get('notes').value;

        if (day !== '' && startTime !== '' && endTime !== '') {
            // set the day of the meeting for both start and end time
            this.start = new Date(day);
            this.end = new Date(day);
            // split hours and minutes
            const [startHour, startMinute] = startTime.split(':');
            const [endHour, endMinute] = endTime.split(':');
            // set hours and minutes for both start and end time
            this.start.setHours(startHour, startMinute);
            this.end.setHours(endHour, endMinute);
        }
        this.notes = notes;

        const meeting: Meeting = {
            hostId: this.musicianId,
            guestId: this.userId,
            startTime: this.start,
            endTime: this.end,
            notes: this.notes,
            accepted: false
        };

        this.service.post(meeting).subscribe(result => {
            console.log(result);
            alert('Propozycja spotkania została wysłana');
        }, err => console.error(err));
    }
}
