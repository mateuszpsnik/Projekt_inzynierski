import { Component, Input, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators, AsyncValidatorFn, AbstractControl } from "@angular/forms";
import { AuthorizeService } from "src/api-authorization/authorize.service";
import { MeetingService } from "./meeting.service";
import { Meeting } from "src/models/meeting";
import { Guid } from "src/models/guid";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Component({
    selector: 'app-meeting-planner',
    templateUrl: './meeting.component.html',
    styleUrls: ['./meeting.component.css']
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
            day: new FormControl('', Validators.required),
            startTime: new FormControl('', Validators.required),
            endTime: new FormControl('', Validators.required),
            notes: new FormControl('')
        }, null, this.isEndTimeInvalid());
    }

    getMeeting(): Meeting {
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

        return meeting;
    }

    onSubmit() {
        const meeting = this.getMeeting();

        this.service.post(meeting).subscribe(result => {
            console.log(result);
            alert('Propozycja spotkania została wysłana');
            this.form.reset();
        }, err => console.error(err));
    }

    isEndTimeInvalid(): AsyncValidatorFn {
        return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
            const meeting = this.getMeeting();
            return this.service.isEndTimeInvalid(meeting)
                        .pipe(map(result => {
                            return (result ? { isEndTimeInvalid: true } : null);
                    }));
        };
    }
}
