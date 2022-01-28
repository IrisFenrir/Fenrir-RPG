namespace IrisFenrir
{
    // 在TKey类型的对象中查找TValue类型的属性
    public delegate TValue SelectHandler<TKey, TValue>(TKey key);
}
