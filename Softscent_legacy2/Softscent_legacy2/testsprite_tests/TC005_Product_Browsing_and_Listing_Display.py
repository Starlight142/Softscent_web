import asyncio
from playwright import async_api
from playwright.async_api import expect

async def run_test():
    pw = None
    browser = None
    context = None
    
    try:
        # Start a Playwright session in asynchronous mode
        pw = await async_api.async_playwright().start()
        
        # Launch a Chromium browser in headless mode with custom arguments
        browser = await pw.chromium.launch(
            headless=True,
            args=[
                "--window-size=1280,720",         # Set the browser window size
                "--disable-dev-shm-usage",        # Avoid using /dev/shm which can cause issues in containers
                "--ipc=host",                     # Use host-level IPC for better stability
                "--single-process"                # Run the browser in a single process mode
            ],
        )
        
        # Create a new browser context (like an incognito window)
        context = await browser.new_context()
        context.set_default_timeout(5000)
        
        # Open a new page in the browser context
        page = await context.new_page()
        
        # Navigate to your target URL and wait until the network request is committed
        await page.goto("http://localhost:62507", wait_until="commit", timeout=10000)
        
        # Wait for the main page to reach DOMContentLoaded state (optional for stability)
        try:
            await page.wait_for_load_state("domcontentloaded", timeout=3000)
        except async_api.Error:
            pass
        
        # Iterate through all iframes and wait for them to load as well
        for frame in page.frames:
            try:
                await frame.wait_for_load_state("domcontentloaded", timeout=3000)
            except async_api.Error:
                pass
        
        # Interact with the page elements to simulate user flow
        # -> Click on the 'สินค้า' (Products) link to navigate to the product listing page.
        frame = context.pages[-1]
        # Click on the 'สินค้า' (Products) link to go to the product listing page.
        elem = frame.locator('xpath=html/body/form/header/nav/div/div/ul/li[2]/a').nth(0)
        await page.wait_for_timeout(3000); await elem.click(timeout=5000)
        

        # --> Assertions to verify final state
        frame = context.pages[-1]
        await expect(frame.locator('text=เปปเปอร์มินต์เฟรช').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เย็นสดชื่นทันที ช่วยให้ตื่นตัวและแก้ปวดหัว').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿5.99').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ลาเวนเดอร์สลีป').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ปรับสมดุลการนอนด้วยกลิ่นลาเวนเดอร์แท้').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿6.99').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').nth(1)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ซิทรัส เอนเนอร์จี').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เติมพลังให้ร่างกายด้วยกลิ่นส้มสดชื่น').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿5.99').nth(1)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').nth(2)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ยาดมสมุนไพรแบบกระปุก').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ตำรับสมุนไพรหมักดั้งเดิม ให้กลิ่นหอมผ่อนคลาย').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿12.99').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ปรุงสูตรเอง').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ยูคาลิปตัส เคลียร์').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ช่วยให้หายใจโล่ง แก้คัดจมูกอย่างได้ผล').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿6.49').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').nth(3)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ตะไคร้หอม เซน').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=สัมผัสความผ่อนคลายเหมือนอยู่ในสปา').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿7.99').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ปรุงสูตรเอง').nth(1)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=Custom Inhaler Blend').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=Create your perfect scent from our selection of organic Thai herbs and essential oils.').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿59.00').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ปรุงสูตรเอง').nth(2)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ยูคาลิปตัสเฟรช').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=กลิ่นหอมสะอาด ช่วยให้หายใจโล่งและสดชื่น').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿5.50').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').nth(4)).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ตะไคร้หอมคาล์ม').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=กลิ่นแฝงความสงบ ช่วยลดความเครียดและไล่แมลง').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿6.00').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=เพิ่มลงตะกร้า').nth(5)).to_be_visible(timeout=30000)
        await asyncio.sleep(5)
    
    finally:
        if context:
            await context.close()
        if browser:
            await browser.close()
        if pw:
            await pw.stop()
            
asyncio.run(run_test())
    