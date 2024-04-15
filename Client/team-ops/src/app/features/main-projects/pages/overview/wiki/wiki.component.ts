import { Component } from '@angular/core';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-wiki',
  templateUrl: './wiki.component.html',
  styleUrl: './wiki.component.scss',
})
export class WikiComponent {
  public Editor = ClassicEditor;
  public onReady(editor: any) {
    console.log('CKEditor5 Angular Component is ready to use!', editor);
  }
  public onChange({ editor }: ChangeEvent) {
    const data = editor.getData();
    console.log(data);
  }
}
