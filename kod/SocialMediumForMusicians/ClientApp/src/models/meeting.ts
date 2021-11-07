export interface Meeting {
    id: string;
    hostId: string;
    guestId: string;
    startTime: Date;
    endTime: Date;
    notes: string;
    accepted: boolean;
}
