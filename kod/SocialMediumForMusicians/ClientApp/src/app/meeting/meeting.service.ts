import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Meeting } from "src/models/meeting";
import { PaginationApiResult } from "src/models/pagination_api_result";

@Injectable({ providedIn: 'root' })
export class MeetingService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    getMeetings(hostId: string, guestId: string, pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult<Meeting>> {
        const url = this.baseUrl + 'api/Meetings/';
        let params = new HttpParams().set('pageIndex', pageIndex.toString())
                                       .set('pageSize', pageSize.toString());
        if (hostId !== null && hostId !== '') {
            params = params.append('hostId', hostId);
        }
        if (guestId !== null && guestId !== '') {
            params = params.append('guestId', guestId);
        }

        return this.http.get<PaginationApiResult<Meeting>>(url, { params });
    }

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

    delete(meeting: Meeting): Observable<Meeting> {
        const url = this.baseUrl + 'api/Meetings/' + meeting.id;
        return this.http.delete<Meeting>(url);
    }

    isEndTimeInvalid(meeting: Meeting): Observable<boolean> {
        const url = this.baseUrl + 'api/Meetings/IsEndTimeInvalid';
        return this.http.post<boolean>(url, meeting);
    }
}
