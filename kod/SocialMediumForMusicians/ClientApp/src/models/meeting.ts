export interface Meeting {
    id?: number;
    hostId: string;
    guestId: string;
    startTime: Date;
    endTime: Date;
    notes: string;
    accepted: boolean;
}
