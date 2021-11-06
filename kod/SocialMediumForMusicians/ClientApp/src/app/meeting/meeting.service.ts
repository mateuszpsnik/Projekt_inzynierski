import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Meeting } from "src/models/meeting";

@Injectable({ providedIn: 'root' })
export class MeetingService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    get(id: number): Observable<Meeting> {
        const url = this.baseUrl + 'api/Meetings/' + id;
        return this.http.get<Meeting>(url);
    }

    put(meeting: Meeting): Observable<Meeting> {
        const url = this.baseUrl + 'api/Meetings/' + meeting.id;
        return this.http.put<Meeting>(url, meeting);
    }

    post(meeting: Meeting): Observable<Meeting> {
        const url = this.baseUrl + 'api/Meetings/';
        return this.http.post<Meeting>(url, meeting);
    }
}
