import { ServerErrorComponent } from './../shared/errors/server-error/server-error.component';
import { Routes } from '@angular/router';
import { HomeComponent } from '../features/home/home.component';
import { ListsComponent } from '../features/lists/lists.component';
import { MemberDetailedComponent } from '../features/members/member-detailed/member-detailed.component';
import { MessagesComponent } from '../features/messages/messages.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MembersEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MembersMessagesComponent } from './members/members-messages/members-messages.component';
import { MembersPhotosComponent } from './members/members-photos/members-photos.component';
import { MembersProfileComponent } from './members/members-profile/members-profile.component';
import { authGuard } from '../core/guards/auth.guard';
import { TestErrorsComponent } from '../features/test-errors/test-errors.component';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MemberListComponent },
      { path: 'members/:id', component: MemberDetailedComponent },
      {
        path: 'member/edit',
        component: MembersEditComponent,
        canDeactivate: [preventUnsavedChangesGuard],
        children: [
          { path: '', redirectTo: 'profile', pathMatch: 'full' },
          {
            path: 'profile',
            component: MembersProfileComponent,
            title: 'Profile',
          },
          {
            path: 'photos',
            component: MembersPhotosComponent,
            title: 'Photos',
          },
          {
            path: 'messages',
            component: MembersMessagesComponent,
            title: 'Messages',
          },
        ],
      },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ],
  },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent },
];
