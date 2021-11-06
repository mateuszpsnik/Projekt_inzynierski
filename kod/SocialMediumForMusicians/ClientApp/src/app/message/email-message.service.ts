import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { EmailMessage } from "src/models/message";

@Injectable({ providedIn: 'root' })
export class EmailMessageService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

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
