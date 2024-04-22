import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateWikiComponent } from './create-update-wiki.component';

describe('CreateUpdateWikiComponent', () => {
  let component: CreateUpdateWikiComponent;
  let fixture: ComponentFixture<CreateUpdateWikiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateUpdateWikiComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateUpdateWikiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
