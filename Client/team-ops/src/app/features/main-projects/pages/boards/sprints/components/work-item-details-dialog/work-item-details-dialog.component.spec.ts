import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemDetailsDialogComponent } from './work-item-details-dialog.component';

describe('WorkItemDetailsDialogComponent', () => {
  let component: WorkItemDetailsDialogComponent;
  let fixture: ComponentFixture<WorkItemDetailsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WorkItemDetailsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WorkItemDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
