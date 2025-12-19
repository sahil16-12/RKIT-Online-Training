/*
  Assignment 8.2
  Topic: Classes, Objects, Static methods
*/

class BankAccount {
  constructor(accountHolder, balance) {
    this.accountHolder = accountHolder;
    this.balance = balance;
  }

  deposit(amount) {
    this.balance += amount;
  }

  withdraw(amount) {
    if (amount > this.balance) {
      console.log("Insufficient balance");
      return;
    }
    this.balance -= amount;
  }

  static info() {
    return "Bank System v1.0";
  }
}

// Create two independent accounts
const account1 = new BankAccount("Sahil", 5000);
const account2 = new BankAccount("Danish", 3000);

// Transactions
account1.deposit(1000);
account2.withdraw(500);

console.log(account1);
console.log(account2);
console.log(BankAccount.info());
