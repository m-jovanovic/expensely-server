import {
  ApplicationRef,
  ComponentFactory,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  Injectable,
  Injector
} from '@angular/core';
import { interval, timer } from 'rxjs';
import { first, take } from 'rxjs/operators';

import { NotificationDialogComponent } from '../components';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationDialogComponentRef: ComponentRef<NotificationDialogComponent>;

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private applicationRef: ApplicationRef,
    private injector: Injector
  ) {}

  notify(message: string, timeout?: number): void {
    const componentRef = this.createNotificationDialogComponent(message);

    this.appendNotificationDialogToView(componentRef);

    this.removeNotificationDialogFromViewOnClose(componentRef, timeout);

    this.notificationDialogComponentRef = componentRef;
  }

  private createNotificationDialogComponent(message: string): ComponentRef<NotificationDialogComponent> {
    const componentFactory: ComponentFactory<NotificationDialogComponent> = this.componentFactoryResolver.resolveComponentFactory(
      NotificationDialogComponent
    );

    const componentRef = componentFactory.create(this.injector);

    componentRef.instance.message = message;

    return componentRef;
  }

  private appendNotificationDialogToView(componentRef: ComponentRef<NotificationDialogComponent>) {
    this.applicationRef.attachView(componentRef.hostView);

    const domElement = (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;

    document.getElementsByTagName('main').item(0).appendChild(domElement);
  }

  private removeNotificationDialogFromViewOnClose(componentRef: ComponentRef<NotificationDialogComponent>, timeout?: number): void {
    if (timeout) {
      const timer$ = timer(timeout).pipe(first());

      timer$.subscribe(() => componentRef?.instance?.closed?.emit());
    }

    const subscription = componentRef.instance.closed.subscribe(() => {
      this.applicationRef.detachView(this.notificationDialogComponentRef.hostView);

      this.notificationDialogComponentRef.destroy();

      subscription.unsubscribe();
    });
  }
}
