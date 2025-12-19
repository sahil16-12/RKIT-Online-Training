/*
  BankAccount module
*/

export class BankAccount {
  constructor(accountHolder, balance) {
    this.accountHolder = accountHolder;
    this.balance = balance;
  }

  deposit(amount) {
    this.balance += amount;
  }

  withdraw(amount) {
    if (amount > this.balance) return;
    this.balance -= amount;
  }

  static info() {
    return "Bank System v1.0";
  }
}
