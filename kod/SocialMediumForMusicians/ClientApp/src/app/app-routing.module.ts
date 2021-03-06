import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "src/api-authorization/authorize.guard";
import { AdminComponent } from "./admin/admin.component";
import { BlockComponent } from "./block/block.component";
import { FavouritesComponent } from "./favourites/favourites.component";
import { HomeComponent } from "./home/home.component";
import { MeetingsComponent } from "./meetings/meetings.component";
import { MessagesThreadComponent } from "./messages-thread/messages-thread.component";
import { MessagesComponent } from "./messages/messages.component";
import { MusicianComponent } from "./musician/musician.component";
import { MusiciansComponent } from "./musicians/musicians.component";
import { ReviewComponent } from "./review/review.component";
import { ReviewsMusicianComponent } from "./reviews-musician/reviews-musician.component";
import { ReviewsUserComponent } from "./reviews-user/reviews-user.component";

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'Musicians', component: MusiciansComponent },
    { path: 'Musician/:id', component: MusicianComponent },
    { path: 'Meetings', component: MeetingsComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Reviews/:id', component: ReviewComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Messages', component: MessagesComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Messages/:id', component: MessagesThreadComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'UserReviews', component: ReviewsUserComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'MusicianReviews', component: ReviewsMusicianComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Favourites', component: FavouritesComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Admin', component: AdminComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Block/:id', component: BlockComponent,
        canActivate: [AuthorizeGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule {}
