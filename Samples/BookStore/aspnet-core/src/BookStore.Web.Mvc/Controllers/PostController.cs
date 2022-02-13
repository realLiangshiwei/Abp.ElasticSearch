using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.ElasticSearch;
using BookStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BookStore.Web.Controllers;

public class PostController : BookStoreControllerBase
{
    private const string IndexName = "post";
    private readonly IElasticsearchManager _elasticsearchManager;

    public PostController(IElasticsearchManager elasticsearchManager)
    {
        _elasticsearchManager = elasticsearchManager;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        await SeedData();

        var data = await _elasticsearchManager.SearchAsync<Post>(IndexName);

        return View(MapResult(data.Hits));
    }

    public async Task<IActionResult> Search(string keyword)
    {
        var search = new SearchDescriptor<Post>();

        search.Query(q => q.MultiMatch(m => m.Fields(f => f.Field(post => post.Title).Field(post => post.Content)).Query(keyword)));

        var data = await _elasticsearchManager.SearchAsync<Post>(IndexName, search, highFields : new[] { "content", "title" });

        return View("index", MapResult(data.Hits));
    }

    private List<PostViewModel> MapResult(IReadOnlyCollection<IHit<Post>> data)
    {
        var result = new List<PostViewModel>();
        foreach (var item in data)
        {
            result.Add(new PostViewModel
            {
                Title = item.Highlight == null || !item.Highlight.ContainsKey("title") ? item.Source.Title : item.Highlight["title"].FirstOrDefault(),
                Content = item.Highlight == null || !item.Highlight.ContainsKey("content") ? item.Source.Title : item.Highlight["content"].FirstOrDefault(),
                Description = item.Highlight == null || !item.Highlight.ContainsKey("description") ? item.Source.Title : item.Highlight["description"].FirstOrDefault(),
                DateTime = item.Source.DateTime,
                Id = item.Source.Id
            });
        }

        return result;
    }

    public async Task SeedData()
    {
        await _elasticsearchManager.CreateIndexAsync<Post>(IndexName);

        var countResponse = await _elasticsearchManager.CountAsync<Post>(IndexName);
        if (countResponse.Count == 0)
        {
            var ran = new Random();
            var list = new List<Post>();

            for (var i = 0; i < 5; i++)
            {
                list.Add(new Post()
                {
                    Title = ".NET Standard 2.0 新版标准：整齐划一的目标" + ran.Next(0, 10),
                    Content =
                        "最近结束的.NET Connect 2016大会上，几位微软MVP针对.NET标准的内容和未来发展谈论了自己的看法。在两个月前公布.NET Standard 2.0时，微软认为新版标准的目标在于为现有的三个主要.NET平台：.NET Framework、.NET Core，以及Xamarin提供一个坚实的底层基础，并为未来满足树莓派或IoT等全新类型设备需求可能需要创建的分支提供支持。\r\n\r\n对开发者来说，目前现有三个分支最主要的问题在于难以清楚地知道每个平台具体有哪些功能可用，这个问题会显得极为不便。.NET开发者GaProgMan认为，这会导致开发者过度使用条件编译（Conditional compilation），此外他还补充说Portable Class Library（PCL）已经不再那么易于移植了，因为开发者无法轻松确保自己需要的API在目标平台上依然可用。根据微软的介绍，使用.NET Standard取代PCL作为编写多平台.NET库的底层基础可以解决这一问题。\r\n\r\n然而微软MVP Rick Strahl指出，.NET Standard应当被视作一种用于描述“至少在API接口方面需要选择哪一具体的实现，例如.NET Core、Mono、Xamarin或.NET 4.6”的规范。换句话说，.NET Standard本身并非一种实现，而是由.NET底层平台实现的。例如他认为，.NET Core实现了当前版本的.NET Standard 1.6版，而他认为.NET Core 1.2将非常接近.NET Standard 2.0，使其成为.NET Standard 1.6的超集。\r\n\r\n为了解释.NET Standard 2.0到底是什么，Strahl将其与核心的.NET Base Class Library（BCL）在核心操作系统、运行时，以及语言服务方面进行了对比。其中包括基本类型系统、运行时的加载和查询操作、网络和文件I/O，以及一些额外的API，例如System.Data。此外还对比了并非.NET Standard标准的一部分，但基于该标准构建的应用程序框架，例如ASP.NET、WinForms、WPF等。从实现的角度来看，.NET Standard采取了与传统.NET略微不同的方法。实际上.NET Standard针对每个特定平台的实现还提供了可充当类型转发器（Type forwarder）的.NET Standard DLL。应用程序只需要引用类型提供程序（Type provider）DLL，即可将引用转发给能提供所需实现的相应程序集（Assembly）。相比.NET程序集，这种做法提供了类似的用户体验，但在实施者（Implementer）方面有很大不同，因为它们可以分别提供独立的程序包，而非像.NET运行时程序包那样提供一个单一的整体。\r\n\r\n.NET Standard 2.0将.NET Standard 1.6 API的范围增大了不止两倍，预计将于2017年1季度末发布，并且有可能在正式发布前首先提供预览版本。",
                    DateTime = DateTime.Now,
                    Description = "最近结束的.NET Connect 2016大会上，几位微软MVP针对.NET标准的内容和未来发展谈论了自己的看法"
                });
                list.Add(new Post()
                {
                    Title = "微软愚人节：6.15英寸Surface Phone搭载Win10亮相" + ran.Next(0, 10),
                    Content =
                        "由于时差的原因，美国时间要比中国晚一些，因此当大家认为愚人节结束的时候，冷不丁地还能被外媒消息吓一跳。而今天发生的微软愚人节全新移动设备把老外都给骗了。可能是大家太希望看到Surface Phone了。\r\n此前IT之家曾报道过，微软商店将首次开卖三星S8/S8+ Microsoft定制版，这令很多用户和粉丝感到不可思议。实际上微软本质上仍然是世界上的软件开发商巨头，移动为先、云为先也体现在应用生态跨平台、跨系统之上。随着Lumia手机的下架，很多人猜测微软移动硬件战略下一步到底怎么走？在今天微软Facebook官方页面出现了一台疑似Surface Phone的小型Win10平板电脑设备，并且配置强劲。该设备搭载完整的Windows10系统，6.15英寸2960 x 1440 2K屏幕，8GB内存，1TB存储，双核Intel Kaby Lake处理器，支持USB Type C、HDMI接口，还有被称为Surface Pro 5连接器的配件。\r\n但在该信息发布后，微软很快就将其删除了，引起了外媒的关注和猜测。最后被证明这只是愚人节活动玩笑。\r\n微软预计将在4月份举行Win10 Surface新品发布会，但不会推出Surface Pro 5和Surface Book 2设备，另外Xbox天蝎座主机可能会提前亮相",
                    DateTime = DateTime.Now,
                    Description = "由于时差的原因，美国时间要比中国晚一些，因此当大家认为愚人节结束的时候，冷不丁地还能被外媒消息吓一跳。而今天发生的微软愚人节全新移动设备把老外都给骗了"
                });
                list.Add(new Post()
                {
                    Title = "今日清明节：寻春莫忘归，纸烛悼先人" + ran.Next(0, 10),
                    Content =
                        "今天是我国二十四节气中的清明节。“清明”有冰雪消融，草木青青，天气清彻明朗，万物欣欣向荣之意。说到清明节，很多人会想起“清明时节雨纷纷，路上行人欲断魂”的名句。是的，清明一到，气温升高，雨量增多，正是春耕春种的大好时节。清明节又叫踏青节，是中国传统节日之一，也是最重要的祭祀节日之一，是祭祖和扫墓的日子。在传统社会中，清明节是一个特别盛大的节日，除了扫墓祭奠、怀念离世亲人，它还是踏青嬉游、亲近大自然的节日。古人根据对日影的观测，在一年中定出24个节点，每一节点指代的那天被称作一个节气。一年中，以立春为起始，清明是第五个时间节点。每年的4月4日或5日，当视太阳到达黄经15度时，便为清明节气。地球绕太阳一圈的时间称为“回归年”或“太阳年”，自清代开始，定以春分点为0度，太阳在黄道上每运行15度定为一个节气，二十四节气因而为24个特定的时刻，而非特定的24天。但一个回归年实际长度为365.2422天，历法上的一年长度则为365天，因此每年会多出0.2422天（相当于5.8小时），节气的特定时刻也会每年“顺延”0.2422天；如此一来，累积4年后几近一天，为修正它，公历历法中有“闰年”制度，每4年会多出2月29日一天。这也是今年清明节在4月4日，而不是4月5日的原因清明节的起源，据传始于古代帝王将相“墓祭”之礼，后来民间亦相仿效，于此日祭祖扫墓，历代沿袭而成为中华民族一种固定的风俗。而清明节纪念祖先的习俗和寒食节有关，寒食节传说是在春秋时代为纪念晋国的忠义之臣介子推而设立的。春秋时期，晋公子重耳为逃避迫害而流亡国外，流亡途中，在一处渺无人烟的地方，又累又饿，无法找到食物，后来多亏随臣介子推“割股充饥”，救了重耳一命。十九年后，重耳做了国君，也就是晋文公。即位后文公重重赏了当初伴随他流亡的功臣，唯独忘了介子推。很多人为介子推鸣不平，但介子推已归隐山林。清明时节，气温变暖，降雨增多，正是春耕春种的大好时节。所以清明对于古代农业生产而言是一个重要的节气。这个时期，我国大部分耕种地区的日平均气温已上升到12℃以上，到处是一片繁忙的春耕景象。华南气候温暖，春意正浓。但常言道：“清明断雪，谷雨断霜。”在清明前后，仍然时有冷空气入侵，甚至使日平均气温连续3天以上低于12℃，造成中稻烂秧和早稻死苗，所以水稻播种、栽插要避开暖尾冷头。在西北高原，牲畜经严冬和草料不足的影响，抵抗力弱，需要严防开春后的强降温天气对老弱幼畜的危害后来晋文公带人去请介子推，然而寻之不得，于是有人献计从三面烧山，逼出介子推。最后大火烧遍，却没见介子推的身影，火熄后，人们才发现背着老母亲的介子推已坐在一棵老柳树下死了。晋文公见状，恸哭。装殓时，从树洞里发现一血书，上写道：“割肉奉君尽丹心，但愿主公常清明。”为纪念介子推，晋文公下令将这一天定为寒食节。第二年晋文公率众臣登山祭奠，发现老柳树死而复活。便赐老柳树为”清明柳“，并晓谕天下，把寒食节的后一天定为清明节。",
                    DateTime = DateTime.Now,
                    Description =
                        "今天是我国二十四节气中的清明节。“清明”有冰雪消融，草木青青，天气清彻明朗，万物欣欣向荣之意。说到清明节，很多人会想起“清明时节雨纷纷，路上行人欲断魂”的名句。是的，清明一到"
                });
            }

            await _elasticsearchManager.BulkAddOrUpdateAsync(IndexName, list);
        }
    }

    public class Post
    {
        public Post()
        {
            Id = Guid.NewGuid();
        }

        [Text(Index = true)] public Guid Id { get; set; }

        [Text] public string Title { get; set; }

        [Text] public string Description { get; set; }

        [Text] public string Content { get; set; }

        [Date] public DateTime DateTime { get; set; }
    }
    
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}
