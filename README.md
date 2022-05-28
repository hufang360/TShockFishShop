# 鱼店

鱼店是一个指令商店，除出售物品外，还出售 召唤boss、召唤入侵、生成NPC、获得buff、调时间 等自定义商品。支持配置购买条件 和 支付条件。

下载地址：
| 文件名  | 适用版本  |
|---|---|
| [FishShop-v1.4.1.dll](https://gitee.com/hufang360/TShockFishShop/attach_files/1077291/download/FishShop_v1.4.1.dll)  | TShock4.5.17 - Terraria1.4.3.6  |
| [FishShop-v1.4.1-1.4.3.2.dll](https://gitee.com/hufang360/TShockFishShop/attach_files/1077297/download/FishShop_v1.4.1_1.4.3.2.dll)  | TShock4.5.13 - Terraria1.4.3.2（建议1.4.3.2的服务器用这个）  |
| [FishShop-v1.4.1-1.4.0.5.dll](https://gitee.com/hufang360/TShockFishShop/attach_files/1077296/download/FishShop_v1.4.1_1.4.0.5.dll)  | TShock4.4.0_Pre12_Terraria1.4.0.5（理论上支持，未做完整测试）  |


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
