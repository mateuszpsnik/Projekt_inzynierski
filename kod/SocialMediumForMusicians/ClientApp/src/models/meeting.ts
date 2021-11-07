export interface Meeting {
    id: string;
    hostId: string;
    guestId: string;
    startTime: Date;
    endTime: Date;
    notes: string;
    accepted: boolean;
    hostName?: string;
    guestName?: string;
    hostImgFilename?: string;
    guestImgFilename?: string;
}
