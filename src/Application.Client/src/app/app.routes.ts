import { Routes } from '@angular/router';
import { authGuard } from '../core/guards/auth.guard';
import { preventUnsavedChangesGuard } from '../core/guards/prevent-unsaved-changes.guard';
import { HomeComponent } from '../features/home/home.component';
import { ListsComponent } from '../features/lists/lists.component';
import { MemberDetailedComponent } from '../features/members/member-detailed/member-detailed.component';
import { MemberEditComponent } from '../features/members/member-edit/member-edit.component';
import { MemberListComponent } from '../features/members/member-list/member-list.component';
import { MemberMessagesComponent } from '../features/members/member-messages/member-messages.component';
import { MemberPhotosComponent } from '../features/members/member-photos/member-photos.component';
import { memberResolver } from '../features/members/member.resolver';
import { MembersProfileComponent } from '../features/members/members-profile/members-profile.component';
import { MessagesComponent } from '../features/messages/messages.component';
import { TestErrorsComponent } from '../features/test-errors/test-errors.component';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';
import { ServerErrorComponent } from './../shared/errors/server-error/server-error.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MemberListComponent },
      {
        path: 'members/:id',
        resolve: { member: memberResolver },
        runGuardsAndResolvers: 'always',
        component: MemberDetailedComponent,
        children: [
          {
            path: '',
            redirectTo: 'profile',
            pathMatch: 'full',
          },
          {
            path: 'profile',
            title: 'Profile',
            component: MembersProfileComponent,
          },
          {
            path: 'photos',
            title: 'Photos',
            component: MemberPhotosComponent,
          },
          {
            path: 'messages',
            title: 'Messages',
            component: MemberMessagesComponent,
          },
        ],
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        canDeactivate: [preventUnsavedChangesGuard],
      },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ],
  },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent },
];
