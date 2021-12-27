import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { EmailMessage } from "src/models/message";
import { PaginationApiResult } from "src/models/pagination_api_result";

@Injectable({ providedIn: 'root' })
export class EmailMessageService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    getMessages(userId: string, pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult<EmailMessage>> {
        const url = this.baseUrl + 'api/EmailMessages/';
        const params = new HttpParams().set('id', userId)
                                       .set('pageIndex', pageIndex.toString())
                                       .set('pageSize', pageSize.toString());
        return this.http.get<PaginationApiResult<EmailMessage>>(url, { params });
    }

    get(id): Observable<EmailMessage> {
        const url = this.baseUrl + 'api/EmailMessages/' + id;
        return this.http.get<EmailMessage>(url);
    }

    put(message: EmailMessage): Observable<EmailMessage> {
        const url = this.baseUrl + 'api/EmailMessages/' + message.id;
        return this.http.put<EmailMessage>(url, message);
    }

    post(message: EmailMessage): Observable<EmailMessage> {
        const url = this.baseUrl + 'api/EmailMessages/';
        return this.http.post<EmailMessage>(url, message);
    }
}
