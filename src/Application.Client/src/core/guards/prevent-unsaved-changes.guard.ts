import { CanDeactivateFn } from '@angular/router';
import { MembersProfileComponent } from '../../features/members/members-profile/members-profile.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<
  MembersProfileComponent
> = (component) => {
  if (component.editForm?.dirty) {
    return confirm(
      'Are you sure you sure you want to continue? Any unsaved changes will be lost'
    );
  }

  return true;
};
