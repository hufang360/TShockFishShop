# 鱼店

鱼店是一个指令商店，除出售物品外，还出售 召唤boss、召唤入侵、生成NPC、获得buff、调时间 等自定义商品。支持配置购买条件 和 支付条件。

下载地址：
| 文件名  | 适用版本  |
|---|---|
| [FishShop-v1.4.3.dll](https://gitee.com/hufang360/TShockFishShop/releases/download/v1.4.3/FishShop-v1.4.3.dll)  | TShock5.0beta - Terraria1.4.4.x  |
* Terraria 1.4.3.x 请下载 1.4.2及之前的版本


<br>

## 指令

```
/fish list，查看商店;
/fish ask <商品编号>，询价;
/fish buy <商品编号> [件数]，购买;

/fish info, 显示钓鱼信息;

/fish reload, 重载配置;
/fish reset, 重置限量记录（1.4）;

/fish special, 查看特别指令（仅管理员）;
/fish finish <次数>，修改自己的渔夫任务完成次数（仅管理员）;
/fish change，更换今天的任务鱼（仅管理员）;
/fish changesuper <物品id|物品名>，指定今天的任务鱼（仅管理员）;
/fish docs，生成参考文档（仅管理员）;

/fish = /fishshop = /fs
```

<br>

## 权限

| 权限 | 说明 |
|---|---|
| fishshop.change | 切换钓鱼任务 |
| fishshop.changesuper | 指定钓鱼任务 |
| fishshop.finish  | 修改钓鱼完成次数 |
| fishshop.reload  | 重载鱼店配置 和 重置限量记录 |
| fishshop.special  | 服主和开发者专用 |
| fishshop.ignore.allowgroup  | 忽略用户组购买限制 |

授权示意（服主默认拥有全部权限）：
```shell
/group addperm default fishshop.change
```

<br>

## 配置说明：

[https://www.yuque.com/hufang/bv/tshock-fish-shop](https://www.yuque.com/hufang/bv/tshock-fish-shop)

<br>

## 支持：
[https://afdian.net/@hufang360](https://afdian.net/@hufang360)
