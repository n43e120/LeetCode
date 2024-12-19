from manim import *


# #https://www.dongaigc.com/a/manim-voiceover-professional-narration
# from manim_voiceover import VoiceoverScene
# from manim_voiceover.services.gtts import GTTSService
# from manim_voiceover.services.azure import AzureService

# class MyAwesomeScene(VoiceoverScene):
#     def construct(self):
#         self.set_speech_service(GTTSService())
#         #self.set_speech_service(AzureService())
#         with self.voiceover(text="Swap A and B.") as tracker:
#             self.play(Create(Circle()), run_time=tracker.duration)


def nextPermutation(nums) -> bool:
	i = j = len(nums) - 1
	while i > 0 and nums[i - 1] >= nums[i]:
		i -= 1
	if i == 0:  # nums are in descending order
		# nums.reverse()
		# return
		return False
	k = i - 1  # find the last "ascending" position
	while nums[j] <= nums[k]:
		j -= 1
	nums[k], nums[j] = nums[j], nums[k]
	l, r = k + 1, len(nums) - 1  # reverse the second part
	while l < r:
		nums[l], nums[r] = nums[r], nums[l]
		l += 1
		r -= 1
	return True


def First(n: int) -> list:
	return list(range(n))


class CreateCircle(Scene):
	def construct(self):
		n = 3
		A = First(n)
		dot = Dot(UP * 2)
		cards = Group()
		# poker unicode https://www.compart.com/en/unicode/block/U+1F0A0

		cards.add(SVGMobject("1f0cf.svg"))
		cards.add(SVGMobject("spade_a.svg"))
		cards.add(SVGMobject("heart_a.svg"))
		cards.add(SVGMobject("club_a.svg"))
		cards.add(SVGMobject("square_a.svg"))
		cards.add(SVGMobject("heart_10.svg"))
		# conside scen is from -6 to 6 unit width
		cards = cards[0:n]
		cards.set_x(0).arrange(buff=0.3)

		#self.play(map(lambda x:FadeIn(x, target_position=dot), cards))
		self.play(map(lambda x:Rotate(x, PI/2, UP), cards))

		newcards = Group()
		for card in cards:
			c = SVGMobject("back.svg")
			c.set_x(card.get_x())
			c.set_y(card.get_y())
			c.set_z(card.get_z())
			c.rotate(PI/2,UP)
			newcards.add(c)
		self.remove(cards)
		oldcards = cards
		cards = newcards
		self.add(cards)
		self.play(map(lambda x:Rotate(x, -PI/2, UP), cards))
	
		B = A.copy()
		not_ended: bool = False
		while True:
			not_ended = nextPermutation(B)
			if not not_ended:
				B = First(n)
			aniList = []
			visited = []
			for iCard in range(0, n):
				if iCard in visited:
					continue
				a = A.index(iCard)
				b = B.index(iCard)  # new position
				if a >= b:  # stay
					continue
				group = VGroup(cards[iCard])
				visited.append(iCard)
				while True:
					icard2 = A[b]
					group.add(cards[icard2])
					visited.append(icard2)
					if B.index(icard2) == a:
						aniList.append(Swap(*group))
						break
					b = B.index(icard2)
			if len(aniList) > 0:
				self.play(aniList, run_time=0.5)
			A = B.copy()
			if not not_ended:
				break

		self.play(map(lambda x:Rotate(x, PI/2, UP), cards))
		for i in range(0,n):
			c2 = newcards[i]
			c = oldcards[i]
			c.set_x(c2.get_x())
			c.set_y(c2.get_y())
			c.set_z(c2.get_z())
		self.remove(cards)
		newcards = cards
		cards = oldcards
		self.add(cards)
		self.play(map(lambda x:Rotate(x, -PI/2, UP), cards))
