# OrleansTest
奥尔良框架测试

初步学习阶段

测试下传说中Actor类框架（奥尔良）对于同Id下能做到单线程的测试用例

模拟一个转账的场景，有一个Account类，它有转账（TransferAsync）和查看当前资产（AssertAsync）方法

然后并发产生转账操作（从1块一直累计到转账100块），最终转账全部结束后账户累计的正确金额是4950( 1 + 2 + 3 + ...99 + 100)

不使用奥尔良直接new一个Account的场景下会发现每次运行结果都不确定，而使用奥尔良则稳定输出4950，证明mailBox之类的机制确实能确保多线程下的一致性
