// src: https://stackoverflow.com/questions/26501688/a-typescript-guid-class

export class Guid {
    static newGuid() {
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        // tslint:disable-next-line: no-bitwise
        const r = Math.random() * 16 | 0,
          // tslint:disable-next-line: no-bitwise
          v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
      });
    }
}
