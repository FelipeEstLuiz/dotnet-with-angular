import { MemberParams } from './../../../types/member-params';
import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { Member } from '../../../types/member';
import { ApiResponse } from '../../../types/api-response';
import { PaginatorComponent } from '../../../shared/paginator/paginator.component';
import { FilterModalComponent } from '../filter-modal/filter-modal.component';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent, PaginatorComponent, FilterModalComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  @ViewChild('filterModel') modal!: FilterModalComponent;
  protected memberService = inject(MemberService);
  protected memberParams = new MemberParams();

  private updateParams = new MemberParams();

  paginatedMember = signal<ApiResponse<Member[]> | null>(null);

  constructor() {
    const filters = localStorage.getItem('filters');

    if (filters) {
      this.memberParams = JSON.parse(filters);
      this.updateParams = JSON.parse(filters);
    }
  }

  async ngOnInit(): Promise<void> {
    await this.loadMembers();
  }

  async loadMembers() {
    const member = await this.memberService.getAll(this.memberParams);

    if (member) this.paginatedMember.set(member);
  }

  async onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.memberParams.pageNumber = event.pageNumber;
    this.memberParams.pageSize = event.pageSize;
    await this.loadMembers();
  }

  openModal() {
    this.modal.open();
  }

  onClose() {
    console.log('Modal close');
  }

  async onFilterChange(data: MemberParams) {
    this.memberParams = { ...data };
    this.updateParams = { ...data };
    await this.loadMembers();
  }

  async resetFilters() {
    this.memberParams = new MemberParams();
    this.updateParams = new MemberParams();
    await this.loadMembers();
  }

  get displayMessage(): string {
    const defaultParams = new MemberParams();

    const filters: string[] = [];

    if (this.updateParams.gender) filters.push(this.updateParams.gender + 's');
    else filters.push('Males', 'Females');

    if (
      this.updateParams.minAge !== defaultParams.minAge ||
      this.updateParams.maxAge !== defaultParams.maxAge
    )
      filters.push(
        ` ages ${this.updateParams.minAge}-${this.updateParams.maxAge}`
      );

    filters.push(
      this.updateParams.orderBy === 'lastActive'
        ? 'Recently active'
        : 'Newest members'
    );

    return filters.length > 0
      ? `Selected: ${filters.join('  | ')}`
      : 'All members';
  }
}
