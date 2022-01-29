# 鱼店

TShock指令商店，通过指令查看和购买配置好的物品/服务。

大致流程：逛店 (/fish list)  -> 询价 (/fish ask)  -> 购买 (/fish buy)


<br>

## 指令

```shell
/fish           主指令，也可以输 /fishshop 或 /fs

/fish help, 帮助;
/fish list [页数]，查看商店，可不输页数;
/fish ask <商品编号>，询价;
/fish buy <商品编号> [件数]，购买，不写件数默认买一件;

/fish search <物品名|物品id>，查询某个物品的解锁条件和出售条件;
/fish info, 查询钓鱼信息;

/fish change，更换今天的任务鱼;
/fish changesuper <物品id|物品名>，指定今天的任务鱼;
/fish finish <次数>，修改自己的渔夫任务完成次数;
```

<br>

## 权限
部分指令需要下面这些权限：
```shell
# 修改钓鱼完成次数
fishshop.finish

# 切换钓鱼任务
fishshop.change

# 指定钓鱼任务
fishshop.changesuper
```

授权示意：
```shell
/group addperm default fishshop.finish
/group addperm default fishshop.change
/group addperm default fishshop.changesuper
```

<br>

## 特殊商品
商品支持配置泰拉物品，物品还支持指定前缀。另外还可配置这些特殊商品：
- 召唤boss，支持boss、怪物 和 npc；
- 下个月相，切换到下个月相，购买两次则切换到下下个月相，切月相时会把时间调到晚上；
- 调白天 / 调晚上，将时间调到白天或晚上；
- 跳过入侵，跳过入侵事件；
- 雨来 / 雨停，开始下雨 和 停止下雨；
- 更换任务鱼，更换今天的任务鱼；
- 烟花起飞，将你送上天，上天前给你放个大烟花；
- 召唤血月 / 跳过血月；


<br>

## 建筑师折扣
建筑师买东西价格打1折（仅货币，非物品），可以将服里的建筑师添加到“builder”用户组，购买东西时会自动折扣。

```shell
# 创建建筑师组
/group add builder

# 继承default 组所有权限
/group parent builder default

# 将玩家（hf）加入到 建筑师组
/user group hf builder
```


<br>

---

<br>

## 配置

<br>



### 结构
给json加注释是为了说明配置方法，实际配置时不要写注释，不然会出错！
```json
{
    "name": "鱼店",     // 店铺名字
    "pageSlots": 40,    // 一页显示几个商品
    "rowSlots": 10,     // 一行显示几个商品

    "unlock":  [           // 鱼店解锁条件，可以配置多个条件
        {
        "name": "渔夫任务",
        "id": 0,
        "stack": 10     // 数量
        }
    ],

    "shop": [             // 商品列表
        {
            "name": "生命水晶", // 物品名字，可不写；
            "id": 29,                   // 物品id，写0时会根据name匹配物品，配置物品是建议使用物品id；
            "stack": 1,                 // 数量
            "prefix": "",              // 词缀，例如 虚幻

            "unlock":  [             // 物品解锁条件，同样可以配置多条
                {
                    "name": "肉后", // 物品名字，可不写；
                    "id": 0,              // 物品id，写0时会根据name匹配j解锁条件；
                    "stack": 1
                },
                {
                    "name": "生命<400", // 生命<400 是商店特定功能，只能填写name来进行配置
                    "id": 0,
                    "stack": 1
                }
            ],
            "cost": [                   // 花费，钱 或 用于交易的物品；同样可以配置多条
                {
                "name": "金币",
                "id": 73,               //物品id，目前只有货币这几种物品支持填写文字，其它的都需要填写id
                "stack": 2          // 数量
                }
            ]
        }
    ]

}
```

<br>

### 解锁条件

可以配置多个解锁条件。

- 击败指定boss后，ID的配置方式是 id=-(2000+NPCID)，NPCID必须是boss的npcid，name的配置方式是id=0，然后name填写 肉后、一王后、三王后、花后、石后、一柱后、四柱后、月后接受这些文字，更多中文boss名后面会一并列出。例如：
  - （name=肉后, id=-2113, statck=1）
  - （name=肉后, id=0, statck=1）
  - （name=击败 血肉墙, id=0, statck=1）

<br>

- 某个NPC活着，ID的配置方式是  id=-(3000+NPCID) ，npcid可以是npc、boss和敌怪，也可以使用name来配置，例如：
  - （name="", id=-3369, statck=1）
  - （name=渔夫 在场, id=0, statck=1）
  - （name=渔夫 活着, id=0, statck=1）

<br>

- 完成多少次 渔夫任务，name写 "渔夫任务" 或 "钓鱼任务"，stack写次数。例如：
  - （name=渔夫任务, id=0, stack=10）。

<br>

- 特定月相，name写 满月、亏凸月、下弦月、残月、新月、娥眉月、上弦月、盈凸月。例如：
  - （name=满月, id=0, statck=1）

<br>

- 血月期间，例如：（name=血月, id=0, stack=1）。
- 雨天期间，例如：（name=雨天, id=0, stack=1）。
- 生命<400，name还写 "血量<400"、"hp<400"，例如：（name=生命<400, id=0, stack=1）。




<br>

<br>

### 商店物品
- 常规物品，name仅可写 铜币、银币 金币 铂金币，除此之外请使用id配置，id即物品id，例如：
  - （name="", id=73, stack=5）            5个 金币
  - （name=金币, id=0, stack=5）            5个 金币
  - （name="", id=29, stack=1） 1个 生命水晶
  - （name="生命水晶", id=29, stack=5） 10个 生命水晶

<br>

- 召唤BOSS/NPC，ID的配置方式是 id=-(1000+NPCID)，npcid可以是npc、boss和敌怪，也可以使用name来配置，例如：
  - （name="", id=-1262, statck=1）    
  - （name=召唤 世纪之花, id=0, statck=10）召唤 10个 世纪之花
  - （name="", id=663, statck=1）    召唤 1个 公主，通过 /fish buy 1 10 来召唤10个
  - （name=召唤 公主, id=0, statck=10）    召唤 10个 公主

<br>

- 下个月相，单次可买2个，买2个的效果是连续切两个月相，例如：
  - （name=下个月相, id=0, statck=1）

<br>

- 更换任务鱼，单次只能买1个，例如（name=更换任务鱼, id=0, statck=1），单次只能更换1次任务鱼；

<br>

- 烟花，单次只能买1个，例如（name=烟花, id=0, statck=1）；
- 烟花起飞，单次只能买1个，例如（name=烟花起飞, id=0, statck=1）；
- 雨来，单次只能买1个，例如（name=雨来, id=0, statck=1）；
- 雨停，单次只能买1个，例如（name=雨停, id=0, statck=1）；
- 调白天，单次只能买1个，例如（name=调晚上, id=0, statck=1）；
- 调晚上，单次只能买1个，例如（name=调晚上, id=0, statck=1）；
- 跳过入侵，单次只能买1个，例如（name=跳过入侵, id=0, statck=1）；
- 召唤血月，单次只能买1个，例如（name=召唤血月, id=0, statck=1）；
- 跳过血月，单次只能买1个，例如（name=关掉血月, id=0, statck=1）；

<br>

- 第一分形，（name="", id=4722, statck=1），游戏里无法获得的道具，本插件安装后解锁服务器端的限制，实际要获得还需解锁玩家端的限制，否则购买了不会显示，当在游戏里执行 /fish list 指令后看到第一分形的图标时，即可正常获得和使用。

<br>

- 无趣弓，（name="", id=3853, statck=1），游戏里无法获得的道具，只有一个名字和图标，商店可以购买，玩家也可以获得，但是没有功能。

<br>

### 交易物
可以配置多条交易物。

- 常规物品，name仅可写 铜币、银币 金币 铂金币，除此之外请使用id配置，id即物品id，例如：
  - （name="", id=73, stack=5）            售价5个 金币

<br>

- 任务鱼，例如：
  - （name=任务鱼, id=0, stack=1）            当天的任务鱼
  - （name=任务鱼, id=0, stack=2）            当天任务鱼2条

<br>

---

<br>


### 语义化配置
物品id、npc id 和 特殊商品id，不好找不符合直觉。因此 商品、解锁条件 和 交易物 都支持语义化配置，只配name，id写0即可。
例如：
（name=召唤向导, id=0, stack=1）
（name=金币, id=0, stack=5）

目前只 能识别 boss名 和 城镇npc名、货币物品名（铜币/银币/金币/铂金币）,要让插件识别其它物品名和npc名，需添加服务器启动参数，添加参数 `-language zh-Hans`，可将服务器语言为中文。


例如：
```shell
"tshock-client\TerrariaServer.exe" -world "2.wld" -lang 7 -language zh-Hans
```


### 目前已能处理的 BOSS名 和 NPC名
```shell
史王、史莱姆王
史后、史莱姆皇后
克眼、克苏鲁之眼
克脑、克苏鲁之脑
世吞、世界吞噬者
骷髅王
巨鹿、鹿角怪
蜂后、蜂王
铁长直、毁灭者、 双子魔眼、 机械骷髅王
光女、光之女皇
猪鲨、猪龙鱼公爵
邪教徒、拜月教邪教徒
月总、月亮领主后
哀木、南瓜王
常绿尖叫怪、 冰雪皇后、冰雪女王、 坦克、圣诞坦克
日耀、日耀柱、日曜、日曜柱、 星旋、星旋柱、 星云、星云柱、 星尘、星尘柱
向导、渔夫、商人、护士、军火商、服装商、爆破专家、树妖、染料商
哥布林工匠、机械师、动物学家、高尔夫球手、油漆工、骷髅商人、发型师、酒馆老板、酒保、旅商、巫医
派对女孩、蒸汽朋克人、机器侠、海盗、公主、圣诞老人、税收官、松露人、巫师
沉睡渔夫、受缚哥布林、受缚机械师、高尔夫球手救援、被网住的发型师、受缚发型师、昏迷男子、受缚巫师、老人
```