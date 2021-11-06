import { Meeting } from "./meeting";
import { Message } from "./message";
import { Review } from "./review";

export interface User {
    id: string;
    blocked: boolean;
    isMusician: boolean;
    name: string;
    profilePicFilename: string;
    description: string;
    hostedMeetings: Array<Meeting>;
    guestMeetings: Array<Meeting>;
    inMessages: Array<Message>;
    outMessages: Array<Message>;
    myReviews: Array<Review>;
    favouriteMusiciansIds: Array<string>;
}
