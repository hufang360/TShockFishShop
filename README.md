# 鱼店

## 指令

```
/fish，主指令，也可以输 /fishshop 或 /fs
/fish help, 帮助;

/fish list，查看商店;
/fish ask <商品编号>，询价;
/fish buy <商品编号> [件数]，购买;

/fish info, 查询钓鱼信息;

/fish change，更换今天的任务鱼;
/fish changesuper <物品id|物品名>，指定今天的任务鱼;
/fish finish <次数>，修改自己的渔夫任务完成次数;
```

<br>

## 权限

| 权限 | 说明 |
|---|---|
| fishshop.change | 切换钓鱼任务 |
| fishshop.changesuper | 指定钓鱼任务 |
| fishshop.finish  | 修改钓鱼完成次数 |

授权示意：
```shell
/group addperm default fishshop.change
```